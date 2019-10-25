.equ color1, colors+0
.equ color2, colors+4
.equ color3, colors+8
.equ color4, colors+12
.thumb
pop	{r3}
add	r0,r2
ldr	r0,[r0]
str	r0,[r1]
str	r4,[r1,#4]
ldr	r0,=#0x84000030
str	r0,[r1,#8]
ldr	r0,[r1,#8]

cmp	r3,#0
beq	getcolor1
cmp	r3,#1
beq	getcolor2
cmp	r3,#2
beq	getcolor3
cmp	r3,#3
beq	getcolor4
b	getcolor1

getcolor1:
ldr	r3,=#0x50001F0
ldr	r2,color1
strh	r2,[r3]
b	end

getcolor2:
ldr	r3,=#0x50001F0
ldr	r2,color2
strh	r2,[r3]
b	end

getcolor3:
ldr	r3,=#0x50001F0
ldr	r2,color3
strh	r2,[r3]
b	end

getcolor4:
ldr	r3,=#0x50001F0
ldr	r2,color4
strh	r2,[r3]
b	end

end:
pop	{r4-r6,pc}
.align
.ltorg
colors:
