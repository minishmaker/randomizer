.equ walletShopSub, walletShopItem+4
.equ boomerangShopItem, walletShopSub+4
.equ boomerangShopSub, boomerangShopItem+4
.equ quiverShopItem, boomerangShopSub+4
.equ quiverShopSub, quiverShopItem+4
.thumb
cmp	r5,#0x64
beq	wallet
cmp	r5,#0x0B
beq	boomerang
cmp	r5,#0x66
beq	quiver
vanilla:
mov	r0,r5
end:
mov	r2,#2
ldr	r3,=#0x80A7410
mov	lr,r3
.short	0xF800
ldr	r3,=#0x8064ED5
bx	r3

wallet:
ldr	r0,=#0x2002EA4
mov	r1,#24
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
ldr	r0,walletShopItem
ldr	r1,walletShopSub
b	end

boomerang:
ldr	r0,=#0x2002EA4
mov	r1,#25
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
ldr	r0,boomerangShopItem
ldr	r1,boomerangShopSub
b	end

quiver:
ldr	r0,=#0x2002EA4
mov	r1,#26
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800
ldr	r0,quiverShopItem
ldr	r1,quiverShopSub
b	end

.align
.ltorg
walletShopItem:
@WORD walletShopItem
@WORD walletShopSub
@WORD boomerangShopItem
@WORD boomerangShopSub
@WORD quiverShopItem
@WORD quiverShopSub
