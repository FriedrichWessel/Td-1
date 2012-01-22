#include <fcntl.h>
#include <termios.h>

int serial_open(char *port, int baud) {
	struct termios options;
	int fd;

	fd = open(port, O_WRONLY);
	if (fd == -1) {
		return fd;
	}

	tcgetattr(fd, &options);
	cfsetispeed(&options, baud);
	cfsetospeed(&options, baud);
	// CSIZE IS SO FUCKING IMPORTANT!
	// e.g.: Mac OS X seems to set CS5 by default, so only setting
	// CS8 additionally doesn’t fix it. Unsetting CSIZE unsets
	// all CS* flags
	options.c_cflag &= ~(PARENB | CSIZE | CSTOPB | CSIZE);
	options.c_cflag |= (CLOCAL | CREAD | CS8);
	options.c_iflag &= ~(ICRNL | INPCK | IXON | IXOFF);
	options.c_oflag &= ~(ONLCR);
	tcsetattr(fd, TCSANOW, &options);
	return fd;
}
