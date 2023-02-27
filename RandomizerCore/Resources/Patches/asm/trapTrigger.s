.equ getRNG, trapTable+4
.equ requirementPrize, getRNG+4
.equ requirementTable, requirementPrize+4
.equ requirementPrizeItem, requirementTable+4
.equ requirementPrizeSub, requirementPrizeItem+4
.thumb
ldr	r3,[r7,#0x30]
mov	r0,#0x32
ldsh	r1,[r3,r0]
ldrh	r0,[r7,#0x08]
sub	r6,r1,r0
push	{r0-r7}

@check if link is in control
ldr	r0,=#0x3003DC0
ldr	r0,[r0]
cmp	r0,#0
bne	end

@check if we should run requirement checks
ldr	r0,requirementPrize
cmp	r0,#0
beq	checkTrap

ldr	r0,=#0x300400B
ldrb	r1,[r0]
cmp 	r1,#0
bne	checkTrap

ldr	r0,=#0x203F1FE
ldrb	r1,[r0]
cmp 	r1,#0
beq	checkTrap

mov	r1,#0
strb	r1,[r0]

@check if we already got the item
ldr	r0,=#0x2002C9E
ldrb	r1,[r0]
mov	r2,#0x40
and	r2,r1
cmp	r2,#0
bne	checkTrap

@check if requirements are met
ldr	r4,requirementTable
loop:
ldr	r0,[r4]
cmp	r0,#0
beq	getItem
mov	lr,r0
.short	0xF800
cmp	r0,#0
beq	checkTrap
add	r4,#4
b	loop

getItem:
@set the flag
ldr	r0,=#0x2002C9E
ldrb	r1,[r0]
mov	r2,#0x40
orr	r1,r2
strb	r1,[r0]

@spawn the item
ldr	r0,requirementPrizeItem
ldr	r1,requirementPrizeSub
mov	r2,#0
ldr	r3,=#0x80A73F8 @CreateItemEntity
mov	lr,r3
.short	0xF800
b	end

checkTrap:
@check if a trap was collected
ldr	r0,=#0x2002B38
ldrb	r1,[r0]
mov	r2,#0xC0
and	r2,r1
cmp	r2,#0
beq	end
mov	r2,#0x3F
and	r1,r2
strb	r1,[r0]

@update the counter
ldr	r0,=#0x203FE00
ldrh	r1, [r0, #8*2]
add	r1, #1
ldr	r2, =#0xFFFF
cmp	r1, r2
bhi	overflow
strh	r1, [r0, #8*2]
overflow:

@trigger it
ldr	r1,=#0x203F1FF
ldrb	r1,[r1]
cmp	r1,#0xFF
bne	notRandom
ldr	r0,trapTable
mov	r1,#0
randomLoop:
ldr	r2,[r0]
add	r1,#1
add	r0,#8
cmp	r2,#0
bne	randomLoop
sub	r1,#1
push	{r1}
ldr	r3,getRNG
mov	lr,r3
.short	0xF800
pop	{r1}
swi	#6
mov	r0,r1
notRandom:
ldr	r0,trapTable
lsl	r1,#3
push	{r0,r1}
ldr	r0,[r0,r1]
cmp	r0,#0
beq	endpop
mov	lr,r0
.short	0xF800
pop	{r0,r1}
add	r0,r1
ldr	r1,[r0,#4]
mov	r0,#0
sub	r0,r1
ldr	r3,=#0x80522BC
mov	lr,r3
.short	0xF800

end:
pop	{r0-r7}
ldr	r0,=#0x80804E5
bx	r0

endpop:
pop	{r0,r1}
b	end
.align
.ltorg
trapTable:
@POIN trapTable
@POIN getRNG
@WORD requirementPrize
@POIN requirementTable
@WORD requirementPrizeItem
@WORD requirementPrizeSub
