.thumb
ldr	r3,=#0x80A217C
mov	lr,r3
.short	0xF800
ldr	r3,=#0x2002CD5
mov	r2,#0x20
ldrb	r1,[r3]
orr	r1,r2
strb	r1,[r3]
mov	r1,#1
strb	r1,[r0,#0x0E]
mov	r1,r0
cmp	r1,#0
beq	goto805D620

ldr	r3,=#0x805D611
bx	r3

goto805D620:
ldr	r3,=#0x805D621
bx	r3
