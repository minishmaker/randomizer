.equ	glitchless, graphics+4
.thumb
strh	r4,[r1,#0x2E]
ldr	r0,=#0x20350F0
mov	r1,#0x80
lsl	r1,#1
push	{r0-r7}
@check if item or quest screen
ldr	r0,=#0x20344A4
ldrb	r0,[r0]
cmp	r0,#1
beq	loadGraphics
cmp	r0,#2
beq	skipbranch1
b	end
skipbranch1:

loadGraphics:
ldr	r7,=#0x2034CB0
@load the icons
ldr	r0,graphics
ldr	r1,=#0x600C020
ldr	r2,=#0x600CC00
loop1:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	loop1

loadpalette:
@load the palette
mov	r6,#0
ldr	r0,=0x85A2590
ldr	r1,=#0x2017800
ldr	r2,=#0x5000160
loop2:
ldr	r3,[r0, r6]
str	r3,[r1, r6]
str	r3,[r2, r6]
add	r6,#4
cmp	r6,#0x20
bne	loop2

@check if quest screen
ldr	r0,=#0x20344A4
ldrb	r0,[r0]
cmp	r0,#2
bne	notQuest

@check if we have the nut
ldr	r3,=#0x2002B48
ldrb	r0,[r3]
mov	r1,#0x40
and	r0,r1
cmp	r0,#0
beq	checkswim
@draw the nut
ldr	r0,=#0x0F2
add	r0,r7
ldr	r1,=#0x202B
bl	drawIcon
ldr	r0,=#0x132
add	r0,r7
ldr	r1,=#0x0031
bl	drawIcon

@check if we have swim butterfly
checkswim:
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
bne	skipbranch2
b	end
skipbranch2:
@draw the butterfly
ldr	r0,=#0x2B0
add	r0,r7
ldr	r1,=#0x1019
bl	drawIcon
b	end

notQuest:
@check if we have dig butterfly
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	noDig
@draw the butterfly
ldr	r0,=#0x1DA
add	r0,r7
ldr	r1,=#0x101F
bl	drawIcon

noDig:
@check if we have shoot butterfly
ldr	r3,=#0x2002B4E
ldrb	r0,[r3]
mov	r1,#1
and	r0,r1
cmp	r0,#0
beq	noShoot
@draw the butterfly
ldr	r0,=#0x2AC
add	r0,r7
ldr	r1,=#0x1025
bl	drawIcon

noShoot:
@check how many clone possibilities we have
mov	r6, #0
ldr	r3, =#0x2002B32
ldrb	r0, [r3, #0]
mov	r1, #0xC0
and	r1, r0
beq	doneone
add	r6, #1
doneone:
ldrb	r0, [r3, #1]
mov	r1, #0x03
and	r1, r0
beq	donetwo
add	r6, #1
donetwo:
mov	r1, #0x30
and	r1, r0
beq	donethree
add	r6, #1
donethree:
cmp	r6, #2
blo	donemulti
@if we have clones, draw select in red
ldr	r0,=#0x3004040
ldr	r0, [r0]
cmp	r0, #0
beq	noclones
ldr	r0,=#0x18C
add	r0,r7
mov	r1,#0x4F
bl	drawIcon
b	donemulti
@otherwise draw the select icon on the sword
noclones:
ldr	r0,=#0x18C
add	r0,r7
mov	r1,#0x0D
bl	drawIcon
donemulti:
@draw the arrow
ldr	r0, =#0x10E
add	r0, r7
mov	r1, #0x4C
strh	r1, [r0, #0x00]
add	r1, #1
strh	r1, [r0, #0x02]
@draw the right number of links
ldr	r6, =#0x203FE00+(10*2)
ldrh	r6, [r6]
lsl	r6, #32 - 2
lsr	r6, #32 - 2
sub	r0, #0x44
mov	r2, #0
cmp	r6, #0
beq	drawOne
cmp	r6, #1
beq	drawTwo
cmp	r6, #2
beq	drawThree
cmp	r6, #3
beq	drawFour

drawOne:
strh	r2, [r0, #0x00]
strh	r2, [r0, #0x02]
strh	r2, [r0, #0x04]
strh	r2, [r0, #0x06]
strh	r2, [r0, #0x08]
strh	r2, [r0, #0x0A]
@erase the arrow
add	r0, #0x44
strh	r2, [r0, #0x00]
strh	r2, [r0, #0x02]
b	doneDrawLinks

drawTwo:
ldr	r1, =#0xB040
strh	r2, [r0, #0x00]
strh	r2, [r0, #0x02]
strh	r1, [r0, #0x04]
add	r1, #1
strh	r1, [r0, #0x06]
add	r1, #1
strh	r1, [r0, #0x08]
strh	r2, [r0, #0x0A]
b	doneDrawLinks

drawThree:
ldr	r1, =#0xB043
strh	r2, [r0, #0x00]
strh	r1, [r0, #0x02]
add	r1, #1
strh	r1, [r0, #0x04]
add	r1, #1
strh	r1, [r0, #0x06]
add	r1, #1
strh	r1, [r0, #0x08]
strh	r2, [r0, #0x0A]
b	doneDrawLinks

drawFour:
ldr	r1, =#0xB047
strh	r2, [r0, #0x00]
strh	r1, [r0, #0x02]
add	r1, #1
strh	r1, [r0, #0x04]
add	r1, #1
strh	r1, [r0, #0x06]
add	r1, #1
strh	r1, [r0, #0x08]
add	r1, #1
strh	r1, [r0, #0x0A]
b	doneDrawLinks

@check if we have boots
doneDrawLinks:
ldr	r3,=#0x2002B37
ldrb	r0,[r3]
mov	r1,#0x0C
and	r1,r0
cmp	r1,#0
beq	noBoots
@draw boots icon
ldr	r0,=#0x292
add	r0,r7
mov	r1,#0x13
bl	drawIcon

noBoots:
@@check if we can swap shield
@ldr	r3,=#0x2002B35
@ldrb	r0,[r3]
@mov	r1,#0x0C
@and	r1,r0
@cmp	r1,#0
@beq	noShield
@mov	r1,#0x30
@and	r1,r0
@cmp	r1,#0
@beq	noShield
@@draw shield icon
@ldr	r0,=#0x24C
@add	r0,r7
@mov	r1,#0x0D
@bl	drawIcon

noShield:
@check if we have ocarina
ldr	r3,glitchless
cmp	r3,#0
beq	noOcarina
ldr	r3,=#0x2002B37
ldrb	r0,[r3]
mov	r1,#0xC0
and	r1,r0
cmp	r1,#0
beq	noOcarina
@draw boots icon
ldr	r0,=#0x2A2
add	r0,r7
mov	r1,#0x37
bl	drawIcon

noOcarina:
@check if we can swap boomerang
ldr	r3,=#0x2002B34
ldrb	r0,[r3]
mov	r1,#0xC0
and	r1,r0
cmp	r1,#0
beq	noBoomerang
ldrb	r0,[r3,#1]
mov	r1,#0x03
and	r1,r0
cmp	r1,#0
beq	noBoomerang
@check if boomerang is in use
mov	r0,#8
mov	r1,#3
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	redBoomerang
@draw boomerang icon
ldr	r0,=#0x1A6
add	r0,r7
mov	r1,#0x01
bl	drawIcon
b	noBoomerang
@draw red boomerang icon
redBoomerang:
ldr	r0,=#0x1A6
add	r0,r7
mov	r1,#0x07
bl	drawIcon

noBoomerang:
@check if we can swap bombs
ldr	r3,=#0x2002B33
ldrb	r0,[r3]
mov	r1,#0xC0
and	r1,r0
cmp	r1,#0
beq	noBomb
ldrb	r0,[r3,#1]
mov	r1,#0x03
and	r1,r0
cmp	r1,#0
beq	noBomb
@check if bomb is in use
mov	r0,#8
mov	r1,#2
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	redBomb
@draw bomb icon
ldr	r0,=#0x266
add	r0,r7
mov	r1,#0x01
bl	drawIcon
b	noBomb
@draw red bomb icon
redBomb:
ldr	r0,=#0x266
add	r0,r7
mov	r1,#0x07
bl	drawIcon

noBomb:
@check if we can swap bow
ldr	r3,=#0x2002B34
ldrb	r0,[r3]
mov	r1,#0x0C
and	r1,r0
cmp	r1,#0
beq	end
mov	r1,#0x30
and	r1,r0
cmp	r1,#0
beq	end
@check if bow is in use
mov	r0,#8
mov	r1,#4
mov	r2,#2
bl	checkItemObject
cmp	r0,#0
bne	redBow
@draw bow icon
ldr	r0,=#0x326
add	r0,r7
mov	r1,#0x01
bl	drawIcon
b	end
@draw red bow icon
redBow:
ldr	r0,=#0x326
add	r0,r7
mov	r1,#0x07
bl	drawIcon

end:
pop	{r0-r7}
ldr	r3,=#0x80A6779
bx	r3

drawIcon:
strh	r1,[r0,#0]
add	r1,#1
strh	r1,[r0,#2]
add	r1,#1
strh	r1,[r0,#4]
add	r1,#1
add	r0,#0x40
strh	r1,[r0,#0]
add	r1,#1
strh	r1,[r0,#2]
add	r1,#1
strh	r1,[r0,#4]
bx	lr

checkItemObject:
push	{r4-r5,lr}
mov	r3,r0
lsl	r2,#3
ldr	r0,=#0x20369F0
ldr	r5,=#0x3003D70
add	r5,r2
ldr	r5,[r5]
add	r2,r0
ldr	r0,[r2,#4]
cmp	r0,r5
beq	noObject
objectLoop:
ldrb	r4,[r0,#8]
cmp	r3,r4
bne	nextObject
ldrb	r4,[r0,#9]
cmp	r1,r4
beq	returnObject
nextObject:
ldr	r0,[r0,#4]
cmp	r0,r5
bne	objectLoop
noObject:
mov	r0,#0
returnObject:
pop	{r4-r5,pc}

.align
.ltorg
graphics:
@POIN graphics
@WOROD glitchless
