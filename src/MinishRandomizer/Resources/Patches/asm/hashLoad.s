.equ hashIconsPalette, hashIconsGraphics+4
.equ hashIconsTable, hashIconsPalette+4
.thumb
mov	r0,#1
ldr	r3,=#0x80A690C
mov	lr,r3
.short	0xF800
push	{r0-r7}

@check if first time
ldr	r0,=#0x2019EE0
ldrb	r0,[r0,#3]
cmp	r0,#7
blo	draw

@load the palette
ldr	r0,hashIconsPalette
ldr	r1,=#0x20176A0
mov	r2,r1
add	r2,#0xC0
paletteLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
blo	paletteLoop

@load the graphics
ldr	r4,=#0x811DBD5
mov	r5,#0
ldr	r6,=#0x600C800
ldr	r7,hashIconsGraphics
graphicsLoop:
ldrb	r0,[r4,r5]
mov	r1,#0x3F
and	r0,r1
mov	r1,#0x80
mul	r0,r1
add	r0,r7
mov	r1,r6
mov	r2,#0
graphicsLoopLoad:
ldr	r3,[r0,r2]
str	r3,[r1,r2]
add	r2,#4
cmp	r2,#0x80
blo	graphicsLoopLoad
add	r6,#0x80
add	r5,#1
cmp	r5,#7
blo	graphicsLoop

@draw the tiles
draw:
ldr	r4,=#0x811DBD5
mov	r5,#0
ldr	r6,=#0x2034CB6
ldr	r7,hashIconsTable
iconsLoop:
ldrb	r0,[r4,r5]
mov	r1,#0x3F
and	r0,r1
lsl	r0,#1
ldrh	r0,[r7,r0]
lsl	r1,r5,#2
orr	r0,r1
add	r0,#0x40
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
add	r0,#1
add	r6,#0x40
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
sub	r6,#0x40
add	r6,#4
add	r5,#1
cmp	r5,#5
bne	np
ldr	r0,=#0x0C5F
mov	r1,#0x40
strh	r0,[r6,r1]
add	r6,#2
np:
cmp	r5,#7
blo	iconsLoop

end:
pop	{r0-r7}
pop	{pc}
.align
.ltorg
hashIconsGraphics:
@POIN hashIconsGraphics
@POIN hashIconsPalette
@POIN hashIconsTable
