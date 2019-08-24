.thumb
push	{r4,r5,lr}
push	{r0-r7}
@check if pressing A/B/L/R
ldr	r0,=#0x3000FF0
ldrh	r1,[r0,#0]
ldrh	r2,[r0,#2]
ldrh	r3,[r0,#4]
orr	r1,r2
orr	r1,r3
ldr	r2,=#0x0303
and	r1,r2
cmp	r1,#0
beq	end
@unset diagonal angle
ldr	r0,=#0x3001174
ldrb	r1,[r0]
mov	r2,#0xFE
and	r1,r2
strb	r1,[r0]
end:
pop	{r0-r7}
mov	r2,r0
mov	r5,#0x80
lsl	r5,#2
and	r0,r5
neg	r0,r0
ldr	r3,=#0x805E899
bx	r3
