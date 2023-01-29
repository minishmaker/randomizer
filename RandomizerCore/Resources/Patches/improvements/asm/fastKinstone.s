.thumb
push	{r4,lr}
mov	r4,r0
mov	r0,#0
ldrb	r1,[r4,#0xA]
cmp	r1,#0x5C
bne	vanilla
ldrb	r1,[r4,#0xB]
cmp	r1,#0x65
blo	fast
cmp	r1,#0x6D
bhi	fast

vanilla:
mov	r0,r4
ldr	r3,=#0x8080E24
mov	lr,r3
.short	0xF800
b	end

fast:
ldr	r0,=#0x103
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
mov	r0,#0

end:
ldr	r3,=#0x8080DFD
bx	r3
