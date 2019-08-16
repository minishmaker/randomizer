.thumb
push	{lr}
ldr	r0,[r0,#0x7C]
ldrb	r0,[r0,#8]
cmp	r0,#0x44
bne	vanilla

ldr	r0,=#0x2002EA4
ldr	r1,=#10
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	invert
mov	r0,#0
b	end
invert:
mov	r0,#1
end:
pop	{pc}

vanilla:
ldr	r3,=#0x8029139
cmp	r0,#0x5C
bx	r3
