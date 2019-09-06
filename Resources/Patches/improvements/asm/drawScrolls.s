.thumb
push	{r4-r7,lr}
mov	r7,r8
push	{r7}
ldr	r6,=#0x3001010
push	{r0-r7}

@load the graphics
ldr	r0,graphics
ldr	r1,=#0x600C400
ldr	r2,=#0x600CC00
loop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
blo	loop

@check if we have fast spin
mov	r0,#0x73
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nospin
ldr	r0,=#0x1020
ldr	r1,=#0x600F8C2
bl	drawIcon

@check if we have fast split
nospin:
mov	r0,#0x74
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nosplit
ldr	r0,=#0x102F
ldr	r1,=#0x600F982
bl	drawIcon

@check if we have long great spin
nosplit:
mov	r0,#0x75
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end
ldr	r0,=#0x103E
ldr	r1,=#0x600F930
bl	drawIcon

end:
pop	{r0-r7}
ldr	r3,=#0x80A5979
bx	r3

drawIcon:
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
bx	lr

.align
.ltorg
graphics:
