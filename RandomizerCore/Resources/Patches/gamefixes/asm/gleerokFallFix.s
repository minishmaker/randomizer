.thumb
cmp	r0,#0
bne	end
ldr	r0,=#0x3003DC0
ldrh	r0,[r0,#0x08]
cmp	r0,#0
bne	end
ldr	r3,=#0x3000BF0
ldrb	r3,[r3,#0x04]
cmp	r3,#0x51
bne	goto8051832
ldr	r3,=#0x3003FB7
ldrb	r3,[r3]
cmp	r3,#2
bhi	goto8051832
mov	r2,#0x80
ldr	r3,=#0x3003F80
strb	r2,[r3,#0x07]

goto8051832:
ldr	r3,=#0x8051832
mov	lr,r3
.short	0xF800

end:
ldr	r3,=#0x805182E
mov	lr,r3
.short	0xF800
