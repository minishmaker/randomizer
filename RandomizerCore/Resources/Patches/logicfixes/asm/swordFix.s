.equ pedestalItems, poin+4
.equ pedestalSpot1Item, pedestalItems+4
.equ pedestalSpot1Sub, pedestalSpot1Item+4
.equ pedestalSpot2Item, pedestalSpot1Sub+4
.equ pedestalSpot2Sub, pedestalSpot2Item+4
.equ pedestalSpot3Item, pedestalSpot2Sub+4
.equ pedestalSpot3Sub, pedestalSpot3Item+4
.thumb
push	{lr}
ldr	r3,[r1]

@check if pedestal items
ldr	r0,pedestalItems
cmp	r0,#0
beq	nopedestal

@check if the pointer is the sword pedestal
ldr	r0,=#0x8013A7E
cmp	r3,r0
beq	pedestal

@check if the pointer is in the list
nopedestal:
ldr	r0,poin
loop:
ldr	r2,[r0]
add	r0,#4
cmp	r2,#0
beq	vanilla
cmp	r2,r3
bne	loop

@if in the list, use the new method
ldrb	r0,[r3,#2]
ldrb	r1,[r3,#3]
pedestalback:
mov	r2,#0	@ldrb	r2,[r3,#4]
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
pop	{pc}

@if not in the list, vanilla
vanilla:
ldr	r1,[r1]
ldrh	r0,[r1,#2]
ldrh	r1,[r1,#4]
ldr	r3,=#0x807C4C4
mov	lr,r3
.short	0xF800
pop	{pc}

pedestal:
@check if first pull
ldr	r0,=#0x2002EA4
ldr	r1,=#31
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	first

@check if second pull
ldr	r0,=#0x2002EA4
ldr	r1,=#32
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	second

@otherwise is third pull
b	third

first:
ldr	r0,pedestalSpot1Item
ldr	r1,pedestalSpot1Sub
b	pedestalback

second:
ldr	r0,pedestalSpot2Item
ldr	r1,pedestalSpot2Sub
b	pedestalback

third:
ldr	r0,pedestalSpot3Item
ldr	r1,pedestalSpot3Sub
b	pedestalback

.align
.ltorg
poin:
@POIN poin
@WORD pedestalItems
@WORD pedestalSpot1Item
@WORD pedestalSpot1Sub
@WORD pedestalSpot2Item
@WORD pedestalSpot2Sub
@WORD pedestalSpot3Item
@WORD pedestalSpot3Sub
