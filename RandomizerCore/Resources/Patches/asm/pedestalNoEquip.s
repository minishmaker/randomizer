.thumb
push	{r4-r5,lr}
push	{r0-r1}
ldr	r0,=#0x3000BF0
ldrb	r0,[r0,#4]
cmp	r0,#0x78
beq	end

vanilla:
pop	{r0-r1}
ldr	r0,[r1,#4]
ldr	r4,=#0xFFFF
lsr	r5,r0,#0x10
ldr	r3,=#0x807EC71
bx	r3

end:
ldr	r0,=#0x2002AF4
mov	r1,#0
strb	r1,[r0]
strb	r1,[r0,#1]
pop	{r0-r1}
pop	{r4-r5,pc}
