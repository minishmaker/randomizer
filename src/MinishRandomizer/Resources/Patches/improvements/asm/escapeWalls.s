.thumb
mov	r0,#0x90
mov	r1,#1
mov	r2,#1
ldr	r3,=#0x807B5BC
mov	lr,r3
mov	r3,#5
.short	0xF800

mov	r0,#0
mov	r1,#1
mov	r2,#1
ldr	r3,=#0x807B5BC
mov	lr,r3
mov	r3,#10
.short	0xF800

ldr	r3,=#0x804D8C1
bx	r3
