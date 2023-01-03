.equ	openDHC, dhcBlocker+4
.thumb
ldr	r0,openDHC
cmp	r0,#0
beq	nounlock
ldr	r0,=#0x2002D0B
ldrb	r1,[r0]
mov	r2,#1
orr	r1,r2
strb	r1,[r0]
nounlock:
ldr	r0,=#0x2002D0B
ldrb	r0,[r0]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	block
ldr	r3,=#0x80AE827
bx	r3

block:
mov	r0,#156
mov	r1,#1
mov	r2,#7
ldr	r3,=#0x807B5BC
mov	lr,r3
mov	r3,#1
.short	0xF800
ldr	r0,dhcBlocker
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x80AE837
bx	r3
.align
.ltorg
dhcBlocker:
