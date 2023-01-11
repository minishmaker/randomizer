#pragma GCC diagnostic ignored "-Wunused-parameter"
#define NAKED __attribute__((naked))

int NAKED __aeabi_idiv(int n, int d) {
    asm("svc #6");
    asm("bx lr");
}

unsigned int NAKED __aeabi_uidiv(unsigned int n, unsigned int d) {
    asm("svc #6");
    asm("bx lr");
}

int NAKED __aeabi_idivmod(int n, int d) {
    asm("svc #6");
    asm("bx lr");
}

unsigned int NAKED __aeabi_uidivmod(unsigned int n, unsigned int d) {
    asm("svc #6");
    asm("bx lr");
}
