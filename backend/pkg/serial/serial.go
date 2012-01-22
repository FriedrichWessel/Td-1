package serial

// #include "serial.h"
import "C"

import (
	"os"
)

const (
	BAUD_115200 = 115200
	BAUD_9600 = 9600
)

func New(port string, baud int) (*os.File, error) {
	fd := int(C.serial_open(C.CString(port), C.int(baud)))
	if fd == -1 {
		return nil, os.EACCES
	}
	return os.NewFile(fd, port), nil
}
