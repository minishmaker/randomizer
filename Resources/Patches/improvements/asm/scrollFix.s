.thumb
strh	r0,[r1]
ldrb	r0,[r4,#0x0A]
push	{r0-r3}
ldr	r3,=#0x807A910
mov	lr,r3
.short	0xF800
pop	{r0-r3}
cmp	r0,#0x43
bgt	end2

end:
ldr	r3,=#0x8083621
bx	r3

end2:
ldr	r3,=#0x808363F
bx	r3
