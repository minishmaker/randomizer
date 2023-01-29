.thumb
push	{r4,lr}
mov	r4,r0
mov	r0,#0
mov	r1,#6
strb	r1,[r4,#0xC]
mov	r1,r4
add	r1,#0x81
strb	r0,[r1]
ldr	r0,[r4,#0x7C]
ldrb	r0,[r0,#8]
cmp	r0,#0x44
bne	end

ldr	r0,=#0x2002EA4
mov	r1,#10
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

end:
ldr	r3,=#0x8029215
bx	r3
