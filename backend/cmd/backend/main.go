package main

import (
	"encoding/json"
	"flag"
	"fmt"
	"log"
	"net/http"
	"net"
	"code.google.com/p/gorilla/gorilla/mux"
	"strconv"
)

var (
	webAddrFlag = flag.String("web", ":8080", "Address to start the webserver on")
	sockAddrFlag = flag.String("sock", ":8181", "Address to start the socket on")
)

const (
	MAX_ID = 4
)

type Command struct {
	ServoID uint8
	Position uint8
}


func main() {
	wc := setupWebInterface()
	sc := setupSocketInterface()
	c := mergeChannels(wc, sc)
	handleCommand(c)
}

func setupWebInterface() (<-chan Command) {
	c := make(chan Command)
	r := new(mux.Router)
	r.HandleFunc("/GET/{id:[0-9]+}/{pos:[0-9]+}", func(w http.ResponseWriter, r *http.Request) {
		log.Printf("New HTTP connection\n")
		vars := mux.Vars(r)
		handleHttpClient(w, vars, c)
	}).Methods("GET")
	go func() {
		e := http.ListenAndServe(*webAddrFlag, r)
		if e != nil {
			panic(e)
		}
	}()
	return c
}

func handleHttpClient(w http.ResponseWriter, vars map[string]string, c chan<- Command) {
		id, e := strconv.Atoi(vars["id"])
		if e != nil || id < 0 || id >= MAX_ID {
			log.Printf("Invalid ID %d\n", id)
			http.Error(w, "Invalid ID", http.StatusPreconditionFailed)
			return
		}

		pos, e := strconv.Atoi(vars["pos"])
		if e != nil || pos < 0 || id > 255 {
			log.Printf("Invalid pos %d\n", pos)
			http.Error(w, "Invalid Pos", http.StatusPreconditionFailed)
			return
		}

		log.Printf("Sending...\n")
		c <- Command {
			ServoID: uint8(id),
			Position: uint8(pos),
		}
		log.Printf("Sent\n")
		http.Error(w, "Success", http.StatusOK)
}

func setupSocketInterface() (<-chan Command) {
	c := make(chan Command)
	addr, e := net.ResolveTCPAddr("tcp4", *sockAddrFlag)
	if e != nil {
		panic(e)
	}
	l, e := net.ListenTCP("tcp4", addr)
	if e != nil {
		panic(e)
	}
	go func() {
		for {
			s, e := l.Accept()
			if e != nil {
				continue
			}
			log.Printf("New TCP connection\n")
			go handleTcpClient(s, c)
		}
	}()
	return c
}

func handleTcpClient(s net.Conn, c chan<- Command) {
	defer s.Close()
	d := json.NewDecoder(s)
	for {
		var cmd Command
		e := d.Decode(&cmd)
		if e != nil {
			log.Printf("Error decoding: %s\n", e)
			return
		}
		c <- cmd
	}
}

func mergeChannels(c1, c2 <-chan Command) (<-chan Command) {
	c := make(chan Command)
	go func() {
		for {
			select {
				case x := <- c1:
					c <- x
				case x := <- c2:
					c <- x
			}
		}
	}()
	return c
}

func handleCommand(c <-chan Command) {
	for cmd := range c {
		fmt.Printf("%d => %d\n", cmd.ServoID, cmd.Position)
	}
}
