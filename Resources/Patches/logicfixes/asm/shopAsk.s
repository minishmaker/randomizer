.equ walletShopSub, walletShopItem+4
.equ boomerangShopItem, walletShopSub+4
.equ boomerangShopSub, boomerangShopItem+4
.equ quiverShopItem, boomerangShopSub+4
.equ quiverShopSub, quiverShopItem+4
.equ getTextOffset, quiverShopSub+4
.equ shootbutterflyCredits, getTextOffset+4
.equ digbutterflyCredits, shootbutterflyCredits+4
.equ swimbutterflyCredits, digbutterflyCredits+4
.equ fastspinCredits, swimbutterflyCredits+4
.equ fastsplitCredits, fastspinCredits+4
.equ longspinCredits, fastsplitCredits+4
.equ greenclockCredits, longspinCredits+4
.equ blueclockCredits, greenclockCredits+4
.equ redclockCredits, blueclockCredits+4
.equ figurineCredits, redclockCredits+4
.equ trapGetIcon, figurineCredits+4
.equ extraText, trapGetIcon+4
.equ progressiveTraps, extraText+4
.thumb
ldrb	r0,[r6,#6]
cmp	r0,#0x64
beq	wallet
cmp	r0,#0x0B
beq	boomerang
cmp	r0,#0x66
beq	quiver
b	vanilla

wallet:
ldr	r0,walletShopItem
ldr	r1,walletShopSub
bl	getText
mov	r1,r3
mov	r2,#0
b	buildText

boomerang:
ldr	r0,boomerangShopItem
ldr	r1,boomerangShopSub
bl	getText
mov	r1,r3
mov	r2,#1
b	buildText

quiver:
ldr	r0,quiverShopItem
ldr	r1,quiverShopSub
bl	getText
mov	r1,r3
mov	r2,#2
b	buildText

buildText:
push	{r4-r7}
ldr	r7,=#0x203F200	@offset
mov	r4,r0		@name
mov	r5,r1		@special
mov	r6,r2		@price
@write the item name to ram
mov	r0,r4
bl	getTextWrap
mov	r1,r7
bl	writeText
mov	r7,r1

@write the price to ram
mov	r0,#0x2C
strb	r0,[r7]
add	r7,#1
mov	r0,#0x20
strb	r0,[r7]
add	r7,#1
mov	r0,#0x02
strb	r0,[r7]
add	r7,#1
mov	r0,#0x01
strb	r0,[r7]
add	r7,#1
cmp	r6,#0
beq	is80
cmp	r6,#1
beq	is300
b	is600
is80:
mov	r0,#0x38
strb	r0,[r7]
add	r7,#1
mov	r0,#0x30
strb	r0,[r7]
add	r7,#1
b	doneprice
is300:
mov	r0,#0x33
strb	r0,[r7]
add	r7,#1
mov	r0,#0x30
strb	r0,[r7]
add	r7,#1
strb	r0,[r7]
add	r7,#1
b	doneprice
is600:
mov	r0,#0x36
strb	r0,[r7]
add	r7,#1
mov	r0,#0x30
strb	r0,[r7]
add	r7,#1
strb	r0,[r7]
add	r7,#1
b	doneprice
doneprice:
mov	r0,#0x02
strb	r0,[r7]
add	r7,#1
mov	r0,#0x00
strb	r0,[r7]
add	r7,#1
mov	r0,#0x2E
strb	r0,[r7]
add	r7,#1
mov	r0,#0x0A
strb	r0,[r7]
add	r7,#1

@write the special line if any
cmp	r5,#2
blo	nospecial
mov	r0,#0x28
strb	r0,[r7]
add	r7,#1
mov	r0,r5
bl	getTextWrap
mov	r1,r7
bl	writeText
mov	r7,r1
sub	r7,#1
ldrb	r0,[r7]
cmp	r0,#0x2E
beq	dot
add	r7,#1
dot:
mov	r0,#0x29
strb	r0,[r7]
add	r7,#1
nospecial:

@add a new line
mov	r0,#0x0A
strb	r0,[r7]
add	r7,#1

@write the buy text to ram
ldr	r0,=#0x2C14
bl	getTextWrap
mov	r1,r7
bl	writeText
mov	r7,r1

@return our special text id
ldr	r0,=#0x2C05
pop	{r4-r7}
b	end

vanilla:
ldr	r3,=#0x8053B68
mov	lr,r3
.short	0xF800

end:
mov	r7,r0
ldr	r3,=#0x8064BBD
bx	r3

writeText:
ldrb	r2,[r0]
strb	r2,[r1]
cmp	r2,#0
beq	endwrite
cmp	r2,#0xF
bhi	notcode
cmp	r2,#3
blo	shortcode
cmp	r2,#5
bhi	shortcode
add	r0,#1
add	r1,#1
ldrb	r2,[r0]
strb	r2,[r1]
shortcode:
add	r0,#1
add	r1,#1
ldrb	r2,[r0]
strb	r2,[r1]
notcode:
add	r0,#1
add	r1,#1
b	writeText
endwrite:
bx	lr

getText:
mov	r3,#0
cmp	r0,#0x1B
bne	nottrap
b	trap
nottrap:
cmp	r0,#0x1C
blo	notBottle
cmp	r0,#0x20
blo	bottle
notBottle:
cmp	r0,#0x05
beq	extra
cmp	r0,#0x67
beq	figurine
cmp	r0,#0x18
beq	greenclock
cmp	r0,#0x19
beq	blueclock
cmp	r0,#0x1A
beq	redclock
cmp	r0,#0x61
beq	shells
cmp	r0,#0x70
beq	shootbutterfly
cmp	r0,#0x71
beq	digbutterfly
cmp	r0,#0x72
beq	swimbutterfly
cmp	r0,#0x73
beq	fastspin
cmp	r0,#0x74
beq	fastsplit
cmp	r0,#0x75
beq	longspin
cmp	r0,#0x5C
beq	kinstone
cmp	r0,#0x50
blo	normal
cmp	r0,#0x53
bhi	normal
b	dungeon

bottle:
cmp	r0, #0x20
beq	normal
ldr	r3,=#0x0400
orr	r0,r3
orr	r3, r1
bx	lr

extra:
ldr	r0,extraText
lsl	r1,#2
ldr	r0,[r1, r0]
bx	lr

normal:
ldr	r1,=#0x0400
orr	r0,r1
bx	lr

figurine:
ldr	r0,figurineCredits
ldr	r3,=#0x0800
orr	r3,r1
bx	lr

greenclock:
ldr	r0,greenclockCredits
bx	lr

blueclock:
ldr	r0,blueclockCredits
bx	lr

redclock:
ldr	r0,redclockCredits
bx	lr

shells:
ldr	r0,=#0x043F
bx	lr

shootbutterfly:
ldr	r0,shootbutterflyCredits
bx	lr

digbutterfly:
ldr	r0,digbutterflyCredits
bx	lr

swimbutterfly:
ldr	r0,swimbutterflyCredits
bx	lr

fastspin:
ldr	r0,fastspinCredits
bx	lr

longspin:
ldr	r0,longspinCredits
bx	lr

fastsplit:
ldr	r0,fastsplitCredits
bx	lr

dungeon:
cmp	r1,#0x18
blo	normal
cmp	r1,#0x1E
bhi	normal
cmp	r1,#0x18
beq	dws
cmp	r1,#0x19
beq	cof
cmp	r1,#0x1A
beq	fow
cmp	r1,#0x1B
beq	tod
cmp	r1,#0x1C
beq	pow
cmp	r1,#0x1D
beq	dhc
cmp	r1,#0x1E
beq	rc
b	normal
dws:
ldr	r3,=#0x720
b	normal
cof:
ldr	r3,=#0x721
b	normal
fow:
ldr	r3,=#0x722
b	normal
tod:
ldr	r3,=#0x723
b	normal
pow:
ldr	r3,=#0x725
b	normal
dhc:
ldr	r3,=#0x727
b	normal
rc:
ldr	r3,=#0x724
b	normal

kinstone:
cmp	r1,#0x65
blo	normal
cmp	r1,#0x6D
bhi	normal
cmp	r1,#0x65
beq	tornado
cmp	r1,#0x66
beq	tornado
cmp	r1,#0x67
beq	tornado
cmp	r1,#0x68
beq	tornado
cmp	r1,#0x69
beq	tornado
cmp	r1,#0x6A
beq	totem
cmp	r1,#0x6B
beq	totem
cmp	r1,#0x6C
beq	totem
cmp	r1,#0x6D
beq	crown
b	normal
tornado:
ldr	r3,=#0x71A
b	normal
totem:
ldr	r3,=#0x70D
b	normal
crown:
ldr	r3,=#0x717
b	normal

getTextWrap:
push	{lr}
ldr	r3,=#0xFFFF
cmp	r0,r3
bhi	isoffset
ldr	r3,getTextOffset
mov	lr,r3
.short	0xF800
pop	{pc}
isoffset:
ldr	r3,=#0x2000007
ldrb	r3,[r3]
lsl	r3,#2
ldr	r0,[r0,r3]
pop	{pc}

trap:
@get the original item
ldrb	r1,[r6,#6]
@find the right offset for this item
ldr	r0,=#0x30017C0
mov	r2,#0x7E
traploop:
ldrb	r3,[r0,r2]
cmp	r3,r1
beq	match
add	r0,#0x88
b	traploop
@get the icon
match:
push	{lr}
ldr	r3,trapGetIcon
mov	lr,r3
.short	0xF800
@check if key/big key
cmp	r0, #0x52
beq	fakeKey
cmp	r0, #0x53
beq	fakeKey
@check if it's in the list
ldr	r2,progressiveTraps
ldrb	r2, [r2, r0]
cmp	r2, #0xFF
beq	noExtra
mov	r1, r2
pop	{r0}
mov	lr, r0
mov	r3, #0
b	extra
noExtra:
ldr	r1,=#0x0400
orr	r0,r1
mov	r3,#0
pop	{pc}

fakeKey:
pop	{r1}
mov	lr, r1
ldr	r3, =#0x726
b	normal

.align
.ltorg
walletShopItem:
@WORD walletShopItem
@WORD walletShopSub
@WORD boomerangShopItem
@WORD boomerangShopSub
@WORD quiverShopItem
@WORD quiverShopSub
@POIN getTextOffset
@POIN shootbutterflyCredits
@POIN digbutterflyCredits
@POIN swimbutterflyCredits
@POIN fastspinCredits
@POIN fastsplitCredits
@POIN longspinCredits
@POIN greenclockCredits
@POIN blueclockCredits
@POIN redclockCredits
@POIN figurineCredits
@POIN trapGetIcon
@POIN extraText
@POIN progressiveTraps
