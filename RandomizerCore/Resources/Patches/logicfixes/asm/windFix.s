.thumb
ldr	r0,=#0x2002EA4
ldr	r1,=#11
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	end
ldr	r3,=#0x804D095
bx	r3

end:
ldr	r3,=#0x804D0B7
bx	r3
