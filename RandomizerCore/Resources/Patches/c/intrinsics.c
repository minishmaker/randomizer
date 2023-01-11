void __aeabi_idivmod(int n, int d) {
    (void)n;
    (void)d;
    asm("svc #6");
}
