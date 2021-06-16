.thumb
push	{r4-r5,lr}
push	{r0-r7}
@check if continue after death
ldr	r0,=#0x30010A4
mov	r1,#0
mov	r2,#8
continueloop:
	ldr	r3,[r0]
	orr	r1,r3
	add	r0,#4
	sub	r2,#1
bne continueloop
cmp	r1,#0
bne	end
@check if quickwarp
ldr	r0,=#0x2000088
ldrb	r0,[r0]
cmp	r0,#0xFF
beq	end
@load the save
ldr	r0,=#0x2000004
ldr	r1,=#0x203FB00
ldrb	r0,[r0]
ldr	r2,=#0x500
mul	r0,r2
ldr	r3,=#0xE001100
add	r0,r3
mov	r3,#0
copyLoop:
ldrb	r4,[r0,r3]
strb	r4,[r1,r3]
add	r3,#1
cmp	r3,r2
bne	copyLoop
@increase load counter
ldr	r0,=#0x203FE00
ldrh	r1,[r0,#10]
ldr	r2,=#0xFFFF
cmp	r1,r2
beq	end
add	r1,#1
strh	r1,[r0,#10]
@and save it
ldrb	r1,[r0,#10]
ldrb	r2,[r0,#11]
ldr	r0,=#0x2000004
ldrb	r0,[r0]
ldr	r3,=#0x500
mul	r3,r0
ldr	r0,=#0xE001400
add	r0,r3
strb	r1,[r0,#10]
strb	r2,[r0,#11]
end:
pop	{r0-r7}
ldr	r0,=#0x3000FD0
ldrb	r5,[r0]
cmp	r5,#0
ldr	r3,=#0x80515F9
bx	r3
