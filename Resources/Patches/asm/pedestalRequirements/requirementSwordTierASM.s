.thumb
push	{lr}
ldr	r0,tier
cmp	r0,#0
bne	np1
mov	r0,#1
np1:
cmp	r0,#5
blo	np2
mov	r0,#6
np2:
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	false

true:
mov	r0,#1
pop	{pc}

false:
mov	r0,#0
pop	{pc}
.align
.ltorg
tier:
