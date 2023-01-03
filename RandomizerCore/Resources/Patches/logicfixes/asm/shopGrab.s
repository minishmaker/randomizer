.thumb
@check if we are in the shop
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
cmp	r1,#0x23
bne	vanilla
ldrb	r1,[r0,#5]
cmp	r1,#0
bne	vanilla

custom:
mov	r0,#0x7E
ldrb	r0,[r6,r0]
strb	r0,[r2,#0x06]
mov	r0,#0
strb	r0,[r2,#0x07]
mov	r0,#1
pop	{r4-r6,pc}

vanilla:
ldrb	r0,[r6,#0x0A]
strb	r0,[r2,#0x06]
ldrb	r0,[r6,#0x0B]
strb	r0,[r2,#0x07]
mov	r0,#1
pop	{r4-r6,pc}
