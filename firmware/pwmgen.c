#include <stdio.h>
#include <avr/sleep.h>
#include <stdint.h>
#include <avr/io.h>
#include <avr/interrupt.h>

#ifdef EXTENDED_RANGE
	#define MAX_PULSE 40000
	#define MIN_PULSE 8000
	#define NUM_PWM 8
#else
	#error "This will not work without implementing shift registers (10x PWM, 8x PINOUT)"
	#define MAX_PULSE 32000
	#define MIN_PULSE 16000
	#define NUM_PWM 10
#endif

#define PWMPORT PORTC
#define PWMDDR	DDRC

// Do some Voodoo
#define PULSE_RANGE (MAX_PULSE - MIN_PULSE)
#define INV_PULSE_RANGE (0 - PULSE_RANGE)
#define PULSE_STEP (PULSE_RANGE / 256)
#define INV_MAX_PULSE (0 - MAX_PULSE)
#define INV_MIN_PULSE (0 - MIN_PULSE)
#define PULSE_OFFSET(x) ((x) * PULSE_STEP)
#define NUM_EDGES (NUM_PWM << 1)
#define UBRR ((F_CPU + (8 * BAUD)) / (16 * BAUD) - 1)

// Bit operations
#define HIGH(x) (((x)>>8)&0xFF)
#define LOW(x) ((x)&0xFF)
#define SET(x, y) (x |= 1<<(y))
#define UNSET(x, y) (x &= ~(1<<(y)))

void uart_init();
void init_pwm();
void set_pwm(uint8_t i, uint8_t p);
void start_timer();
int main();

// {Servo#1_FallingEdge, Servo#2_RisingEdge, ...}
uint16_t edges[NUM_EDGES] ;
uint8_t counter ;


ISR(TIMER1_OVF_vect) {
	uint8_t lsreg = SREG;
	uint8_t pwm ;

	cli();
	pwm = (counter>>1);

	// Even => Falling edge
	if(counter % 2 == 0) {
		UNSET(PWMPORT, pwm);
	} else {
		SET(PWMPORT, (pwm+1)%NUM_PWM);
	}
	counter = (counter+1)%NUM_EDGES;
	TCNT1 = edges[counter];
	SREG = lsreg;
}

void uart_init() {
	UBRRH = HIGH(UBRR);
	UBRRL = LOW(UBRR);

	// Enable Transceiver
	UCSRB = (1 << TXEN) | (1 << RXEN);
	// 8N1
	UCSRC = (1 << URSEL) | (1 << UCSZ1) | (1 << UCSZ0);
}

void init_pwm() {
	uint8_t i ;
	for(i = 0; i < NUM_PWM; i++)
		set_pwm(i, 0);
	return;
}

void set_pwm(uint8_t i, uint8_t p) {
	uint16_t t = PULSE_OFFSET(p);
	uint8_t index = (i<<1);

	// Set falling edge
	edges[index+0] = INV_MIN_PULSE - t;
	// Set next risign edge
	edges[index+1] = INV_PULSE_RANGE + t;
}

void start_timer() {
	counter = 0;
	TCCR1A = 0;
	TCCR1B = 1<<CS10;
	TIMSK |= 1<<TOIE1;
	TCNT1 = edges[0];
}

uint8_t uart_recv() {
	while (!(UCSRA & (1<<RXC)));
	return UDR;
}

int main() {
	uint8_t c1, c2;

	cli();
	uart_init();
	sei();

	init_pwm();

	PWMDDR=0xFF;
	PWMPORT=0x01;

	start_timer();

	while(1) {
		c1 = uart_recv();
		if(c1 == 255)
			continue;
		c2 = uart_recv();
		set_pwm(c1, c2);
	}
	return 0;
}
