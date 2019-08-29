.thumb
ldrh	r0,[r1,#8]
ldr	r3,=#0x200008C
ldr	r2,[r3]
ldr	r3,=#0x812743C
cmp	r2,r3
beq	stats
ldr	r3,=#0x8127370
cmp	r2,r3
bhs	vanilla
ldr	r3,=#0x81270DC
cmp	r2,r3
blo	vanilla
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
mov	r2,#8
and	r2,r3
cmp	r2,#0
bne	skip
ldr	r3,=#0x3000FF0
ldrh	r2,[r3,#2]
ldrh	r3,[r3,#4]
orr	r3,r2
mov	r2,#1
and	r2,r3
cmp	r2,#0
bne	special
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
ldr	r2,=#0x100
and	r2,r3
cmp	r2,#0
beq	vanilla
cmp	r0,#16
bls	special
sub	r0,#16
b	store
vanilla:
sub	r0,#1
b	store
special:
mov	r0,#0
b	store
skip:
ldr	r2,=#0x8127370
ldr	r3,=#0x200008C
str	r2,[r3]
mov	r0,#0
store:
strh	r0,[r1,#8]
lsl	r0,#0x10
ldr	r3,=#0x80A2F01
bx	r3

stats:
push	{r0-r7}
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
mov	r2,#8
and	r2,r3
cmp	r2,#0
bne	endStats
ldr	r4,=#0x3000FF0
ldr	r5,=#0x20000B0
ldr	r6,routines
@check if we are changing pages
@L first
ldrh	r3,[r4,#2]
ldr	r2,=#0x200
and	r3,r2
cmp	r3,#0
beq	checkR
@change page left
ldrb	r0,[r5]
cmp	r0,#0
beq	findLastLoop
sub	r0,#1
b	storePage
findLastLoop:
lsl	r1,r0,#2
add	r1,#4
ldr	r1,[r6,r1]
cmp	r1,#0
beq	storePage
add	r0,#1
b	findLastLoop
checkR:
@and now R
ldr	r3,=#0x3000FF0
ldrh	r3,[r4,#2]
ldr	r2,=#0x100
and	r3,r2
cmp	r3,#0
beq	runPage
@change page right
ldrb	r0,[r5]
lsl	r1,r0,#2
add	r1,#4
ldr	r2,[r6,r1]
cmp	r2,#0
bne	incrasePage
mov	r0,#0
b	storePage
incrasePage:
add	r0,#1
storePage:
@store the page
strb	r0,[r5]
mov	r0,#0
strb	r0,[r5,#1]
strh	r0,[r5,#2]
str	r0,[r5,#4]
str	r0,[r5,#8]
str	r0,[r5,#12]
runPage:
@run the page
ldrb	r0,[r5]
lsl	r0,#2
ldr	r0,[r6,r0]
mov	lr,r0
.short	0xF800
mov	r0,#0x10
pop	{r0-r7}
b	store

endStats:
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
@clean page data
ldr	r1,=#0x20000B0
mov	r0,#0
str	r0,[r1]
str	r0,[r1,#4]
str	r0,[r1,#8]
str	r0,[r1,#12]
@restore io
ldr	r0,=#0x3000F50
ldr	r1,=#0x640
strh	r1,[r0]
ldr	r1,=#0x1E4D
strh	r1,[r0,#0x14]
ldr	r1,=#0x1DC3
strh	r1,[r0,#0x20]
ldr	r1,=#0x1E03
strh	r1,[r0,#0x2C]
pop	{r0-r7}
mov	r0,#0
b	store

.align
.ltorg
routines:

