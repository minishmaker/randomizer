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
ldr	r2,=#0x600C800
loop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	loop

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
@check if we have boots
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
