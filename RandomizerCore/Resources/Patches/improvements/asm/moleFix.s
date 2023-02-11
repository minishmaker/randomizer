.thumb
push	{r4-r7,lr}
@stop running
ldr	r0,=#0x3003F80
ldr	r1,[r0,#0x30]
ldr	r2,=#0x400
mvn	r2,r2
and	r1,r2
str	r1,[r0,#0x30]

@check if this is a mole cave
ldr	r3,=#0x3000BF0
ldrb	r0,[r3,#4]
ldr	r1,=#0x8052278
ldr	r1,[r1]
lsl	r0,#2
ldrb	r0,[r1,r0]
mov	r1,#0x20
and	r0,r1
cmp	r0,#0
beq	vanilla

@check if this is the last cave we were in, or if we have never been in one
ldrb	r1,[r3,#4]
ldr	r2,=#0x2002EBA
ldrb	r0,[r3,#4]
ldrb	r1,[r2,#0]
cmp	r1,#0
beq	match1
cmp	r0,r1
bne	end
match1:
ldrb	r0,[r3,#5]
ldrb	r1,[r2,#1]
cmp	r0,r1
bne	end
match2:
ldrb	r0,[r3,#4]
ldrb	r1,[r3,#5]
strb	r0,[r2,#0]
strb	r1,[r2,#1]
b	vanilla

end:
ldrb	r0,[r3,#4]
ldrb	r1,[r3,#5]
strb	r0,[r2,#0]
strb	r1,[r2,#1]
pop	{r4-r7,pc}

vanilla:
ldr	r0,=#0x3004030
ldr	r1,=#0x3000BF0
ldrb	r0,[r0,#0x0A]
ldrb	r1,[r1,#0x04]
ldr	r3,=#0x807FD53
bx	r3
