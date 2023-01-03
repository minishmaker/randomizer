.equ dungeonDisplayTable, dungeonDisplayGraphics+4
.equ dungeonMapsTable, dungeonDisplayTable+4
.equ dungeonDisplayKeys, dungeonMapsTable+4
.thumb
push	{r4-r7}

ldr	r0,=#0x20350F0
mov	r1,#0x80
lsl	r1,#1
ldr	r3,=#0x801D668
mov	lr,r3
.short	0xF800

clean:
ldr	r0,=#0x2034CB0
ldr	r1,=#0x440
add	r1,r0
mov	r2,#0
cleanLoop:
str	r2,[r0]
add	r0,#4
cmp	r0,r1
bne	cleanLoop

@check current map mode
ldr	r2,=#0x20344A4
ldrb	r2,[r2]
cmp	r2,#5
beq	dungeon
cmp	r2,#4
bne	end

@update dungeon map based on current square
ldr	r3,=#0x2000083
ldrb	r0,[r3]
ldr	r1,dungeonMapsTable
ldrb	r0,[r1,r0]
ldr	r3,=#0x2033A93
strb	r0,[r3]

@find out if this map is in our list
ldr	r0,dungeonDisplayTable
mov	r1,#0xFF
and	r4,r1
loop:
ldrb	r1,[r0]
cmp	r1,#0
beq	end
cmp	r1,r4
beq	match
add	r0,#2
b	loop

match:
ldrb	r4,[r0,#1]
sub	r4,#0x17
ldr	r5,=#0x2034CB0
ldr	r6,=#0x2002E9C
ldr	r7,=#0x2002EAC

@load the graphics
ldr	r0,=#0x600C020
ldr	r1,dungeonDisplayGraphics
ldr	r2,=#0x600CC00
graphics:
ldr	r3,[r1]
str	r3,[r0]
add	r0,#4
add	r1,#4
cmp	r0,r2
bne	graphics

@draw the key
ldr	r0,=#0x344
add	r0,r5
mov	r1,#1
bl	draw

@draw the map if unlocked
ldrb	r0,[r7,r4]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	noMap
ldr	r0,=#0x2F2
add	r0,r5
ldr	r1,=#0x4007
bl	draw

noMap:
@draw the compass if unlocked
ldrb	r0,[r7,r4]
mov	r1,#2
and	r0,r1
cmp	r0,#0
beq	noCompass
ldr	r0,=#0x372
add	r0,r5
ldr	r1,=#0x13
bl	draw

noCompass:
@draw the big key if unlocked
ldrb	r0,[r7,r4]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	noBig
ldr	r0,=#0x3F2
add	r0,r5
ldr	r1,=#0x100D
bl	draw

noBig:
@draw key ammount
ldrb	r0,[r6,r4]
cmp	r0,#9
bls	np
mov	r0,#9
np:
mov	r1,#6
mul	r0,r1
mov	r1,#0x19
add	r1,r0
ldr	r0,=#0x3C4
add	r0,r5
bl	draw
b	end

end:
pop	{r4-r7}
ldr	r3,=#0x80A6807
bx	r3

dungeon:
ldr	r4,=#0x2033A90
ldrb	r4,[r4,#3]
ldr	r5,=#0x2034CB0
ldr	r6,=#0x2002E9C
ldr	r7,=#0x2002EAC

@load the graphics
ldr	r0,=#0x600C020
ldr	r1,dungeonDisplayKeys
ldr	r2,=#0x600CC00
graphics2:
ldr	r3,[r1]
str	r3,[r0]
add	r0,#4
add	r1,#4
cmp	r0,r2
bne	graphics2

@draw the key icon
mov	r0,#0x88
add	r0,r5
mov	r1,#1
bl	draw

@draw the key ammount
ldrb	r0,[r6,r4]
cmp	r0,#9
bls	np2
mov	r0,#9
np2:
mov	r1,#6
mul	r0,r1
mov	r1,#7
add	r1,r0
mov	r0,#0x8E
add	r0,r5
bl	draw
b	end

draw:
strh	r1,[r0]
add	r1,#1
strh	r1,[r0,#2]
add	r1,#1
strh	r1,[r0,#4]
add	r0,#0x40
add	r1,#1
strh	r1,[r0]
add	r1,#1
strh	r1,[r0,#2]
add	r1,#1
strh	r1,[r0,#4]
bx	lr
.align
.ltorg
dungeonDisplayGraphics:
