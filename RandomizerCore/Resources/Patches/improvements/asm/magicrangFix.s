.thumb
pop	{r3}

ldr	r0, =#0x300116C
ldrb	r0, [r0]
cmp	r0, #1
bne	end
mov	r0, #0x80
orr	r1, r0
strb	r1, [r3, #0x0A]
ldrb	r1, [r3, #0x0B]
orr	r0, r1
strb	r0, [r3, #0x0B]
end:
pop	{r4-r5, pc}