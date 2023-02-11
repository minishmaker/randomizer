.thumb
push	{lr}
mov	r2,r0
ldrb	r1,[r2]
mov	r0,#0xFC
and	r0,r1
cmp	r0,#0xC
beq	goto802919E

ldrb	r0,[r2,#8]
cmp	r0,#0x44
bne	vanilla

ldr	r0,=#0x2002EA4
ldr	r1,=#10
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
pop	{pc}

goto802919E:
ldr	r3,=#0x802919F
bx	r3

vanilla:
ldr	r3,=#0x802917D
bx	r3
