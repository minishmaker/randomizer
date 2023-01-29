.thumb
ldr	r2,=#0x2000080
ldr	r1,[r2,#0x0C]
push	{r0-r7}
ldr	r0,=#0x8127388
cmp	r1,r0
blo	end
ldr	r0,=#0x8127430
cmp	r1,r0
bhi	end
cmp	r1,r0
beq	restore
@move bg 1
ldr	r0,=#0x3000F68
mov	r1,#0x18
strh	r1,[r0]
@change the colors
ldr	r0,=#0x50001E0
ldr	r1,=#0x7249	@team
strh	r1,[r0,#02]
ldr	r1,=#0x7E5F	@special thanks
strh	r1,[r0,#04]
ldr	r1,=#0x0CAF
strh	r1,[r0,#14]
ldr	r1,=#0x10D7
strh	r1,[r0,#16]
ldr	r1,=#0x03DF	@contributors
strh	r1,[r0,#20]
ldr	r1,=#0x5910	@links
strh	r1,[r0,#22]
ldr	r1,=#0x4E75
strh	r1,[r0,#24]
ldr	r1,=#0x631A
strh	r1,[r0,#26]
b	end
restore:
@move bg 1
ldr	r0,=#0x3000F68
mov	r1,#0
strh	r1,[r0]
@restore the colors
ldr	r0,=#0x50001E0
ldr	r1,=#0x0E06
strh	r1,[r0,#02]
ldr	r1,=#0x0F28
strh	r1,[r0,#04]
ldr	r1,=#0x0CAF
strh	r1,[r0,#14]
ldr	r1,=#0x10D7
strh	r1,[r0,#16]
ldr	r1,=#0x03DF
strh	r1,[r0,#20]
ldr	r1,=#0x318E
strh	r1,[r0,#22]
ldr	r1,=#0x4E75
strh	r1,[r0,#24]
ldr	r1,=#0x631A
strh	r1,[r0,#26]
@restore wallet color
ldr	r3,=#0x2017880
ldr	r2,=#0x2002AE8
ldrb	r2,[r2]
cmp	r2,#0
beq	green
cmp	r2,#1
bne	red
b	blue
green:
ldr	r1,=#0x0E06
strh	r1,[r0,#02]
strh	r1,[r3,#02]
ldr	r1,=#0x0F28
strh	r1,[r0,#04]
strh	r1,[r3,#04]
b	end
blue:
ldr	r1,=#0x5163
strh	r1,[r0,#02]
strh	r1,[r3,#02]
ldr	r1,=#0x7E65
strh	r1,[r0,#04]
strh	r1,[r3,#04]
b	end
red:
ldr	r1,=#0x0CAF
strh	r1,[r0,#02]
strh	r1,[r3,#02]
ldr	r1,=#0x10D7
strh	r1,[r0,#04]
strh	r1,[r3,#04]
end:
pop	{r0-r7}
ldrb	r0,[r1]
mov	r3,#0
strb	r0,[r2,#5]
strb	r3,[r2,#6]
ldr	r3,=#0x80A3179
bx	r3
.align
.ltorg
