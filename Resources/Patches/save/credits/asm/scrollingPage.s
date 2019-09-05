.equ bgscrollbotPalette, bgscrollbotImage+4
.equ bgscrolltopImage, bgscrollbotPalette+4
.equ bgscrolltopPalette, bgscrolltopImage+4
.equ bgscrolltopscroll, bgscrolltopPalette+4
.thumb
push	{r4-r7,lr}
mov	r7,r0

@check if first frame
ldr	r4,=#0x20000B0
ldrb	r0,[r4,#1]
cmp	r0,#0
beq	skipbranch1
b	buttons
skipbranch1:

@clean page data
mov	r0,#0
strh	r0,[r4,#6]
str	r0,[r4,#8]
str	r0,[r4,#12]
mov	r0,#1
strb	r0,[r4,#1]
strb	r0,[r4,#10]

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
ldr	r1,=#0x1F0E
strh	r1,[r0,#8]
ldr	r1,=#0x1E43
strh	r1,[r0,#0x14]
ldr	r1,=#0x1D45
strh	r1,[r0,#0x20]
ldr	r1,=#0x1C08
strh	r1,[r0,#0x2C]
ldr	r1,=#0x100
strh	r1,[r0,#0x30]

@load the bottom graphics
ldr	r0,bgscrollbotImage
ldr	r1,=#0x6000000
ldr	r2,=#0x6005000
graphicsLoopBot:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	graphicsLoopBot

@load the top graphics
ldr	r0,bgscrolltopImage
ldr	r1,=#0x6005000
ldr	r2,=#0x600A000
graphicsLoopTop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	graphicsLoopTop

@load scroll thingy
ldr	r0,bgscrolltopscroll
ldr	r1,=#0x600A000
ldr	r2,=#0x600C000
graphicsLoopScroll:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	graphicsLoopScroll

@load the debug font
ldr	r0,=#0x85C2F70
ldr	r1,=#0x600C400
ldr	r2,=#0x600CC00
debug:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	debug

@load the bottom palette
ldr	r0,bgscrollbotPalette
ldr	r1,=#0x50001A0
ldr	r2,=#0x50001C0
paletteLoopBot:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	paletteLoopBot
mov	r0,#0
ldr	r1,=#0x5000000
strh	r0,[r1]

@load the top palette
ldr	r0,bgscrolltopPalette
ldr	r1,=#0x50001C0
ldr	r2,=#0x50001E0
paletteLoopTop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	paletteLoopTop
mov	r0,#0
ldr	r1,=#0x5000000
strh	r0,[r1]

@draw the bottom layer
ldr	r0,=#0x600F000
ldr	r1,=#0x600F500
ldr	r2,=#0xD000
bgdrawloopbot:
strh	r2,[r0]
add	r2,#1
add	r0,#2
cmp	r0,r1
bne	bgdrawloopbot

@draw the top layer
ldr	r0,=#0x600E800
ldr	r1,=#0x600ED00
ldr	r2,=#0xE080
bgdrawlooptop:
strh	r2,[r0]
add	r2,#1
add	r0,#2
cmp	r0,r1
bne	bgdrawlooptop

@draw the scrollbar indicator and set the line
ldr	r0,=#0x600E000
ldr	r1,=#0x600E800
ldr	r2,=#0x200
bgdrawloopscroll:
strh	r2,[r0]
add	r0,#2
cmp	r0,r1
bne	bgdrawloopscroll
ldr	r0,=#0xE100
ldr	r1,=#0x600E0F6
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
mov	r0,#0
strh	r0,[r4,#6]

@count the number of lines and save it for future reference
mov	r0,r7
mov	r2,#0
ammountLoop:
ldr	r1,[r0]
cmp	r1,#0
beq	doneammount
add	r2,#1
add	r0,#12
b	ammountLoop
doneammount:
strh	r2,[r4,#8]
sub	r2,#3
strh	r2,[r4,#12]
b	end

buttons:
@increase or decrease current line
checkButtons:
ldr	r0,=#0x3000FF0
ldrh	r1,[r0,#2]
ldrh	r0,[r0,#4]
orr	r0,r1
mov	r2,#0x40
and	r2,r0
cmp	r2,#0
bne	decreaseLine
mov	r2,#0x80
and	r2,r0
cmp	r2,#0
bne	increaseLine
b	scroll

decreaseLine:
ldrh	r0,[r4,#6]
cmp	r0,#0
beq	scroll
sub	r0,#1
strh	r0,[r4,#6]
mov	r0,#1
strb	r0,[r4,#10]
b	scroll

increaseLine:
ldrh	r0,[r4,#6]
ldrh	r1,[r4,#12]
cmp	r0,r1
beq	scroll
add	r0,#1
strh	r0,[r4,#6]
mov	r0,#1
strb	r0,[r4,#10]
b	scroll

@set bg scroll
scroll:
ldrh	r0,[r4,#6]	@current line
cmp	r0,#0
beq	highestbar
ldrh	r1,[r4,#12]	@max line
cmp	r0,r1
beq	lowestbar
swi	#6		@divided
mov	r5,r1		@remainder, % to scroll
mov	r0,#0x64		@difference between highest and lowest scroll position (0x9C = bottom, 0x100 = top)
lsl	r0,#20
ldrh	r1,[r4,#12]	@max line
swi	#6		@divided
mov	r6,r0
mul	r6,r5		@ammount to scroll
lsr	r6,#20
mov	r0,r6
ldr	r1,=#0x100
sub	r1,r0
ldr	r0,=#0x3000F50
strh	r1,[r0,#0x30]
b	end
highestbar:
ldr	r0,=#0x100
ldr	r1,=#0x3000F50
strh	r0,[r1,#0x30]
b	end
lowestbar:
mov	r0,#0x9C
ldr	r1,=#0x3000F50
strh	r0,[r1,#0x30]
b	end

end:
pop	{r4-r7,pc}
.align
.ltorg
bgscrollbotImage:
@POIN bgscrollbotImage
@POIN bgscrollbotPalette
@POIN bgscrolltopImage
@POIN bgscrolltopPalette
@POIN bgscrolltopscroll
