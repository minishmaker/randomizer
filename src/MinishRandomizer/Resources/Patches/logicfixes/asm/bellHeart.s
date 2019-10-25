.thumb
ldr	r3,=#0x80A217C
mov	lr,r3
.short	0xF800
mov	r2,#0x86
mov	r3,#0xCD
strh	r3,[r0,r2]
mov	r1,r0
cmp	r1,#0
beq	goto805D620

ldr	r3,=#0x805D611
bx	r3

goto805D620:
ldr	r3,=#0x805D621
bx	r3
