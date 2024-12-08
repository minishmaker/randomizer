.equ shopShield, shopFloor+4
.equ shopMirrorShield, shopShield+4
.equ shopConsumables, shopMirrorShield+4
.equ shopBuy1, shopConsumables+4
.equ shopBuy2, shopBuy1+4
.equ shopBuy3, shopBuy2+4
.equ shopBuy4, shopBuy3+4
.thumb
push	{r4-r7,lr}

@this has a regular flag so it should just not spawn if it has been collected
ldr	r0,shopFloor
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

@check if we ever found a mirror shield
ldr	r0,=#0x2002EA4
ldr	r1,=#30
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	noMirrorShield
ldr	r0,shopMirrorShield
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
b	noShield
noMirrorShield:

@check if we ever found a shield
ldr	r0,=#0x2002EA4
ldr	r1,=#23
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	noShield
ldr	r0,shopShield
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
noShield:

@these just always spawn
ldr	r0,shopConsumables
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

@check if we purchased this item
ldr	r0,=#0x2002EA4
ldr	r1,=#24
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	no1
ldr	r0,shopBuy1
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
no1:

@check if we purchased this item
ldr	r0,=#0x2002EA4
ldr	r1,=#25
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	no2
ldr	r0,shopBuy2
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
no2:

@check if we purchased this item
ldr	r0,=#0x2002EA4
ldr	r1,=#26
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	no3
ldr	r0,shopBuy3
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
no3:

@check if we purchased this item
ldr	r0,=#0x2002EA4
ldr	r1,=#34
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	no4
ldr	r0,shopBuy4
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
no4:

end:
pop	{r4-r7,pc}
.align
.ltorg
shopFloor:
@POIN shopFloor
@POIN shopShield
@POIN shopMirrorShield
@POIN shopConsumables
@POIN shopBuy1
@POIN shopBuy2
@POIN shopBuy3
@POIN shopBuy4
