.thumb
push	{r4-r6, lr}
mov	r5, r0
ldr	r0, =#0x3001160
ldrb	r0, [r0, #0x0C]
cmp	r0, #7
beq	no
ldr	r0, =#0x2034350
ldrb	r0, [r0, #0x06]
ldr	r3, =#0x8081398
mov	lr, r3
.short	0xF800

no:
pop	{r4-r6, pc}