.thumb
push	{r4,r5,lr}
push	{r1-r7}
@check if in a hole
ldr	r1,=#0x300116C
ldrb	r1,[r1]
cmp	r1,#0x1A
bne	end
@check if holding up or down
mov	r1,#0xC0
and	r1,r0
cmp	r1,#0
beq	end
mov	r0,r1
end:
pop	{r1-r7}
mov	r2,r0
mov	r5,#0x80
lsl	r5,#2
and	r0,r5
neg	r0,r0
ldr	r3,=#0x805E899
bx	r3
