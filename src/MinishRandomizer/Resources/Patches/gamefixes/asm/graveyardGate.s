.thumb
push	{r4,lr}

@check if we ever talked to dampe
ldr	r0,=#0x2002CE9
ldrb	r0,[r0]
mov	r1,#2
and	r0,r1
cmp	r0,#0
beq	NoDampe

@otherwise continue
mov	r0,#0x20
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
ldr	r3,=#0x804BCED
bx	r3

NoDampe:
ldr	r0,=#0x80D8804
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x804BD31
bx	r3

.align
.ltorg
poin:
