.thumb
push	{r4,lr}
mov	r4,r0
cmp	r4,#0
beq	end

@check if transparency stuff is over
ldrb	r0,[r4,#0x0E]
cmp	r0,#4
bne	end
ldrb	r0,[r4,#0x0F]
cmp	r0,#4
bne	end

@set transparency
ldr	r0,=#0x3000FB4
mov	r1,#0
strh	r1,[r0]
ldr	r1,=#0x3648
strh	r1,[r0,#2]
ldr	r1,=#0x0E04
strh	r1,[r0,#4]
ldr	r0,=#0x3001000
mov	r1,#0
strb	r1,[r0]

end:
cmp	r4,#0
bne	goto5A8A0
ldr	r3,=#0x805A87D
bx	r3
goto5A8A0:
ldr	r3,=#0x805A8A1
bx	r3
