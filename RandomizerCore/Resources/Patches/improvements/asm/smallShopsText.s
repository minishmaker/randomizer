.equ bottleScrubSub, bottleScrubItem+4
.equ gripScrubItem, bottleScrubSub+4
.equ gripScrubSub, gripScrubItem+4
.equ goronMerchantCustomSets, gripScrubSub+4
.equ getTextOffset, goronMerchantCustomSets+4
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
.equ kinstoneText, extraText+4
.equ progressiveTraps, kinstoneText+4
.thumb
ldrh	r1,[r4,#8]
ldr	r3,=#0x2D07
cmp	r3,r1
beq	witchShop
ldr	r3,=#0x2910
cmp	r3,r1
beq	bottleShop
ldr	r3,=#0x290C
cmp	r3,r1
beq	gripShop
ldr	r3,=#0x2C1C
cmp	r3,r1
bne	branchtoend

@check current Goron Merchant set number
push	{r0-r3}
ldr	r1,=#0x2002CA3
ldrb	r3,[r1]
mov	r2,#0
mov	r0,#0x40
tst	r3,r0
beq	result
add	r2,#1
mov	r0,#0x80
tst	r3,r0
beq	result
add	r2,#1
ldrb	r3,[r1,#1]
mov	r0,#0x01
tst	r3,r0
beq	result
add	r2,#1
mov	r0,#0x02
tst	r3,r0
beq	result
add	r2,#1

result:
ldr	r1,goronMerchantCustomSets
cmp	r1,r2
pop	{r0-r3}
bhi	goronShop

branchtoend:
b	end

witchShop:
ldr	r1,=#0x2C05
push	{r0-r7}
ldr	r1,=#0x80F94D7
ldrb	r0,[r1]
ldrb	r1,[r1,#1]
bl	getText
mov	r1,r3
mov	r2,#0
b	buildText

bottleShop:
ldr	r1,=#0x2C05
push	{r0-r7}
ldr	r0,bottleScrubItem
ldr	r1,bottleScrubSub
bl	getText
mov	r1,r3
mov	r2,#1
b	buildText

gripShop:
ldr	r1,=#0x2C05
push	{r0-r7}
ldr	r0,gripScrubItem
ldr	r1,gripScrubSub
bl	getText
mov	r1,r3
mov	r2,#2
b	buildText

goronShop:
ldr	r1,=#0x2C05
push	{r0-r7}
ldr	r1,=#0x2034356
ldrb	r0,[r1]
ldrb	r1,[r1,#1]
bl	getText
mov	r1,r3
mov	r2,#3
b	buildText

buildText:
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
mov	r0,#','
strb	r0,[r7]
add	r7,#1
mov	r0,#' '
strb	r0,[r7]
add	r7,#1
mov	r0,#0x02
strb	r0,[r7]
add	r7,#1
mov	r0,#0x01
strb	r0,[r7]
add	r7,#1
cmp	r6,#0
beq	is60
cmp	r6,#1
beq	is20
cmp	r6,#2
beq	is40
cmp	r6,#3
beq	pricefrommessage
b	isFree
is60:
mov	r0,#'6'
strb	r0,[r7]
mov	r0,#'0'
strb	r0,[r7, #1]
add	r7,#2
b	doneprice
is20:
mov	r0,#'2'
strb	r0,[r7]
mov	r0,#'0'
strb	r0,[r7, #1]
add	r7,#2
b	doneprice
is40:
mov	r0,#'4'
strb	r0,[r7]
mov	r0,#'0'
strb	r0,[r7, #1]
add	r7,#2
b	doneprice
pricefrommessage:
push	{r1-r4}
ldr	r3,=#0x2000060
ldr	r3,[r3]
cmp	r3,#100
blo	twodigits
threedigits:
mov	r0,r3
mov	r1,#100
svc	#6 @division
add	r0,#'0'
strb	r0,[r7]
mov	r3,r1
add	r7,#1
twodigits:
mov	r0,r3
mov	r1,#10
svc	#6 @division
add	r0,#'0'
strb	r0,[r7]
add	r1,#'0'
strb	r1,[r7, #1]
add	r7,#2
pop	{r1-r4}
b	doneprice
isFree:
sub	r7, #4
mov	r0, #'?'
strb	r0, [r7]
mov	r0,#0x0A
strb	r0,[r7, #1]
add	r7, #2
b	donefree

doneprice:
mov	r0,#0x02
strb	r0,[r7]
add	r7,#1
mov	r0,#0x00
strb	r0,[r7]
add	r7,#1
mov	r0,#'.'
strb	r0,[r7]
add	r7,#1
mov	r0,#0x0A
strb	r0,[r7]
add	r7,#1

@write the special line if any
donefree:
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
cmp	r6,#3
beq	goronbuy
cmp	r6,#0
beq	witchbuy
ldr	r0,=#0x2901
b	buytext
goronbuy:
ldr	r0,=#0x89DB8F4
mov	r1,r7
bl	writeText
b	buydone
witchbuy:
ldr	r0,=#0x2D00
buytext:
bl	getTextWrap
mov	r1,r7
bl	writeText
mov	r7,r1
buydone:
pop	{r0-r7}
b	end

end:
ldr	r3,=#0x805E93C
mov	lr,r3
.short	0xF800
mov	r3,r4
add	r3,#0x5C
ldr	r0,=#0x80560B7
bx	r0

writeText:
ldrb	r2,[r0]
strb	r2,[r1]
cmp	r2,#0
beq	endwrite
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
cmp	r1,#0x75
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

commonkinstone:
ldr	r0,kinstoneText
sub	r1,#0x6E
lsl	r1,#2
ldr	r0,[r1, r0]
bx	lr

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
ldr	r0,=#0x3001160
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
bottleScrubItem:
@WORD bottleScrubItem
@WORD bottleScrubSub
@WORD gripScrubItem
@WORD gripScrubSub
@WORD goronMerchantCustomSets
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
@POIN kinstoneText
@POIN progressiveTraps
