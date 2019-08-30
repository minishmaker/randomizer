.equ collectionPageGraphics, collectionPageData+4
.equ collectionPagePalette, collectionPageGraphics+4
.equ collectionPageBigGraphics, collectionPagePalette+4
.equ collectionPageBackgroundGraphics, collectionPageBigGraphics+4
.equ collectionPageBackgroundPalette, collectionPageBackgroundGraphics+4
.thumb
push	{r4-r7,lr}
ldr	r4,=#0x20000B0
ldrb	r0,[r4,#1]
cmp	r0,#0
beq	skipbranch1
b	end
skipbranch1:
mov	r0,#1
strb	r0,[r4,#1]
@clean the background
swi	#5
ldr	r0,=#0x600E000
ldr	r1,=#0x6010000
mov	r2,#0
cleanbgloop:
str	r2,[r0]
add	r0,#4
cmp	r0,r1
bne	cleanbgloop

@set the background io stuff
ldr	r0,=#0x3000F50
ldr	r1,=#0xF40
strh	r1,[r0]
ldr	r1,=#0x1F07
strh	r1,[r0,#8]
ldr	r1,=#0x1E42
strh	r1,[r0,#0x14]
ldr	r1,=#0x1D42
strh	r1,[r0,#0x20]
ldr	r1,=#0x1C00
strh	r1,[r0,#0x2C]
ldr	r0,=#0x3000FB0
mov	r1,#0
strh	r1,[r0,#6]

@load the graphics
ldr	r0,collectionPageGraphics
ldr	r1,=#0x6000000
ldr	r2,=#0x6004C00
graphicsLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	graphicsLoop

@load the big graphics
ldr	r0,collectionPageBigGraphics
ldr	r1,=#0x6004000
ldr	r2,=#0x6004C00
biggraphicsLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	biggraphicsLoop

@load the big graphics
ldr	r0,collectionPageBigGraphics
ldr	r1,=#0x6004C00
ldr	r2,=#0x600C000
backgroundgraphicsLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	backgroundgraphicsLoop


@load the palettes
ldr	r0,collectionPagePalette
ldr	r1,=#0x20176A0
ldr	r2,=#0x2017880
paletteLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	paletteLoop
mov	r0,#0
ldr	r1,=#0x20176A0
strh	r0,[r1]

@load the bg palette
ldr	r0,collectionPageBackgroundPalette
ldr	r1,=#0x2017840
ldr	r2,=#0x2017880
bgpaletteLoop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	bgpaletteLoop
mov	r0,#0
ldr	r1,=#0x20176A0
strh	r0,[r1]

@draw the hearts
ldr	r0,=#0x600E840
ldr	r1,=#0x2002AEB
ldrb	r1,[r1]
lsr	r1,#3
cmp	r1,#0
beq	endHearts
cmp	r1,#10
blo	nphearts
mov	r1,#10
nphearts:
bl	drawHearts
ldr	r0,=#0x600E880
ldr	r1,=#0x2002AEB
ldrb	r1,[r1]
lsr	r1,#3
cmp	r1,#10
bls	endHearts
sub	r1,#10
bl	drawHearts
b	endHearts
drawHearts:
ldr	r2,=#0xF1F5
strh	r2,[r0]
add	r2,#1
add	r0,#2
drawHeartsLoop:
strh	r2,[r0]
add	r0,#2
sub	r1,#1
cmp	r1,#0
bne	drawHeartsLoop
bx	lr
endHearts:

@draw tingle
ldr	r0,=#0x2002B41
ldrb	r0,[r0]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	noTingle
ldr	r0,=#0x4200
ldr	r1,=#0x600E800
mov	r2,#17
mov	r3,#5
bl	drawBigSquare
noTingle:

@draw carlov
ldr	r0,=#0x2002B41
ldrb	r0,[r0]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
beq	noCarlov
ldr	r0,=#0x4210
ldr	r1,=#0x600E800
mov	r2,#17
mov	r3,#11
bl	drawBigSquare
noCarlov:

@draw totem stone
ldr	r0,=#0x2002C81
ldrb	r1,[r0]
mov	r2,#0x40
and	r2,r1
cmp	r2,#0
beq	noTotem
mov	r2,#0x80
and	r2,r1
cmp	r2,#0
beq	noTotem
ldrb	r1,[r0,#1]
mov	r2,#1
and	r2,r1
cmp	r2,#0
beq	noTotem
ldr	r0,=#0x4220
ldr	r1,=#0x600E800
mov	r2,#13
mov	r3,#4
bl	drawBigSquare
noTotem:

@draw king stone
ldr	r0,=#0x2002C83
ldrb	r0,[r0]
mov	r1,#8
and	r0,r1
cmp	r0,#0
beq	noKing
ldr	r0,=#0x4230
ldr	r1,=#0x600E800
mov	r2,#13
mov	r3,#8
bl	drawBigSquare
noKing:

@draw tornado stone
ldr	r0,=#0x2002CD9
ldrb	r0,[r0]
mov	r1,#0xF8
and	r0,r1
cmp	r0,r1
bne	noTornado
ldr	r0,=#0x4240
ldr	r1,=#0x600E800
mov	r2,#13
mov	r3,#12
bl	drawBigSquare
noTornado:

@draw the icons
swi	#5
ldr	r4,collectionPageData
iconLoop:
ldrh	r0,[r4]
cmp	r0,#0
beq	endIcons
@check if we own the item
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	grey
@draw the bottom layer
ldrh	r0,[r4,#2]
ldr	r1,=#0x600E800
ldrb	r2,[r4,#6]
ldrb	r3,[r4,#7]
bl	drawSquare
@draw the top layer
ldrh	r0,[r4,#4]
cmp	r0,#0
beq	next
ldr	r1,=#0x600F000
ldrb	r2,[r4,#6]
ldrb	r3,[r4,#7]
bl	drawSquare
next:
add	r4,#12
b	iconLoop
grey:
@draw the bottom layer
ldrh	r0,[r4,#8]
cmp	r0,#0
beq	next
ldr	r1,=#0x600E800
ldrb	r2,[r4,#6]
ldrb	r3,[r4,#7]
bl	drawSquare
@draw the top layer
ldrh	r0,[r4,#10]
cmp	r0,#0
beq	next
ldr	r1,=#0x600F000
ldrb	r2,[r4,#6]
ldrb	r3,[r4,#7]
bl	drawSquare
b	next
endIcons:

@draw the dig butterfly
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	noDig
ldr	r0,=#0x1195
b	wasDig
noDig:
ldr	r0,=#0x6195
wasDig:
ldr	r1,=#0x600F000
mov	r2,#5
mov	r3,#6
bl	drawSquare

@draw the shoot butterfly
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	noShoot
ldr	r0,=#0x1195
b	wasShoot
noShoot:
ldr	r0,=#0x6195
wasShoot:
ldr	r1,=#0x600F000
mov	r2,#11
mov	r3,#9
bl	drawSquare

@draw the swim butterfly
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
beq	noSwim
ldr	r0,=#0x1195
b	wasSwim
noSwim:
ldr	r0,=#0x616F
wasSwim:
ldr	r1,=#0x600F000
mov	r2,#27
mov	r3,#10
bl	drawSquare

@draw the bomb capacity
@check if we own any type of bombs
mov	r0,#0x07
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	hasBomb
mov	r0,#0x08
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	noBomb
hasBomb:
ldr	r0,=#0x2002AEE
ldrb	r0,[r0]
cmp	r0,#4
bhs	bomb99
cmp	r0,#3
beq	bomb50
cmp	r0,#2
beq	bomb30
cmp	r0,#1
beq	bomb10
b	bomb00
bomb99:
ldr	r0,=#0x1CB
b	drawBomb
bomb50:
ldr	r0,=#0x1B9
b	drawBomb
bomb30:
ldr	r0,=#0x1B0
b	drawBomb
bomb10:
ldr	r0,=#0x1A7
b	drawBomb
bomb00:
ldr	r0,=#0x19E
b	drawBomb
drawBomb:
ldr	r1,=#0x600E216
bl	drawSquareOffset
noBomb:

@draw the arrow capacity
@check if we own any type of bow
mov	r0,#0x09
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	hasBow
mov	r0,#0x0A
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	noBow
hasBow:
ldr	r0,=#0x2002AEF
ldrb	r0,[r0]
cmp	r0,#3
bhs	arrow99
cmp	r0,#2
beq	arrow70
cmp	r0,#1
beq	arrow50
b	arrow30
arrow99:
ldr	r0,=#0x1CB
b	drawArrow
arrow70:
ldr	r0,=#0x1C2
b	drawArrow
arrow50:
ldr	r0,=#0x1B9
b	drawArrow
arrow30:
ldr	r0,=#0x1B0
b	drawArrow
drawArrow:
ldr	r1,=#0x600E2D6
bl	drawSquareOffset
noBow:

@draw wallet
ldr	r0,=#0x2002AE8
ldrb	r0,[r0]
mov	r1,#9
mul	r0,r1
ldr	r1,=#0xF1D1
add	r0,r1
ldr	r1,=#0x600EA62
bl	drawSquareOffset

@draw background
ldr	r0,=#0x600F800
ldr	r1,=#0x600FD00
ldr	r2,=#0xD0C0
bgdrawloop:
strh	r2,[r0]
add	r2,#1
add	r0,#2
cmp	r0,r1
bne	bgdrawloop

end:
pop	{r4-r7,pc}

drawSquare:
lsl	r2,#1
add	r1,r2
mov	r2,#0x40
mul	r2,r3
add	r1,r2
drawSquareOffset:
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
bx	lr

drawBigSquare:
lsl	r2,#1
add	r1,r2
mov	r2,#0x40
mul	r2,r3
add	r1,r2
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
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
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
bx	lr

.align
.ltorg
collectionPageData:
@POIN collectionPageData
@POIN collectionPageGraphics
@POIN collectionPagePalette
@POIN collectionPageBigGraphics
@POIN collectionPageBackgroundGraphics
@POIN collectionPageBackgroundPalette
