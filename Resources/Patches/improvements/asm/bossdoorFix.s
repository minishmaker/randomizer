.thumb

ldr	r1, =#0x3000BE3
ldrb	r1, [r1]
cmp	r1, #0
beq	vanilla
mov	r0, #0
pop	{r4-r6, pc}

vanilla:
ldrh	r1, [r0]
mov	r0, #0x88
and	r0, r1
cmp	r0, #0
ldr	r3, =#0x8077E64
mov	lr, r3
.short	0xF800
