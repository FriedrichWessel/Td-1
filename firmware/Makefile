PROG=/dev/cu.SLAB_USBtoUART8
#PROG=/dev/ttyk.usbserial
PROGNAME=avr910
#PROGNAME=stk500v2
NAME=pwmgen
FREQ=16000000
BAUDRATE=115200
#MODE=EXTENDED_RANGE
MODE=NORMAL_RANGE
MCU=atmega8
PART=m8
#MCU=atmega8515
#PART=m8515
CFLAGS=-O2
CFILES=$(NAME).c
all:
	avr-gcc $(CFLAGS)  -I/usr/avr/include -L/usr/avr/lib -mmcu=$(MCU) -D$(MODE) \
		-DBAUD=$(BAUDRATE)UL -DF_CPU=$(FREQ)L -Wall $(CFILES) -o $(NAME).elf
	avr-objcopy -O ihex $(NAME).elf $(NAME).hex

run:
	#git tag -f surma/burned
	avrdude -P $(PROG) -p $(PART) -c $(PROGNAME) -U flash:w:$(NAME).hex
fuses:
	avrdude -P $(PROG) -p $(PART) -c $(PROGNAME) -U hfuse:w:0x`cat fuses | cut -c1-2`:m
	avrdude -P $(PROG) -p $(PART) -c $(PROGNAME) -U lfuse:w:0x`cat fuses | cut -c3-4`:m
clean:
	-@rm *.hex *.elf *.S
.PHONY: fuses
