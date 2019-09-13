.equ pots1, pots
.equ pots2, pots+4
.thumb
push	{lr}
mov	r0,#0x77
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	vaati

nuts:
ldr	r0,pots1
b	loadPots

vaati:
ldr	r0,pots2
b	loadPots

loadPots:
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

mov	r0,#0x77
ldr	r3,=#0x807C5F4
mov	lr,r3
ldr	r3,=#0x804D71D
bx	r3
.align
.ltorg
pots:
