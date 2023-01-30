.thumb
ldr	r0,=#0x2002CA5
ldr	r0,[r0]
mov	r1,#0x78
and	r0,r1
cmp	r0,#0x48
bne	notFinalRound

mov	r0,#0x4F
ldr	r3,=#0x807C654 @ read flag
mov	lr,r3
.short	0xF800
cmp	r0,#0x00
bne	alreadyGotPrize

mov	r0,#0x4F
ldr	r3,=#0x807C728 @ set flag
mov	lr,r3
.short	0xF800
b	notFinalRound

alreadyGotPrize:
mov	r6,#0x01

notFinalRound:
ldr	r3,=#0x80A0C81
bx	r3
