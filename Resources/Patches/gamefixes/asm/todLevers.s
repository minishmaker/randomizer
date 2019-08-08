.thumb
push	{r4-r7}
ldr	r4,=#0x2002C9C
cmp	r0,r4
bne	nottod
ldr	r6,=#0x776
cmp	r1,r6
bne	nottod
istod:
ldr	r4,=#0x2002D8A
ldrb	r4,[r4]
mov	r5,#0x10
and	r4,r5
cmp	r4,#0
beq	end

nottod:
lsr	r2,r1,#3
add	r3,r0,r2
mov	r2,#7
and	r1,r2
mov	r2,#1
lsl	r2,r1
ldrb	r0,[r3]
mov	r1,r0
orr	r1,r2
strb	r1,[r3]
and	r0,r2
end:
pop	{r4-r7}
bx	lr
