.thumb
push	{r4,lr}
add	sp,#-4
ldr	r3,=#0x8052248 @ AreaIsOverworld (what we overwrote)
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	return

@ only spawn bird if at least one wind crest is activated
ldr	r0,=#0x2002A40
ldr	r0,[r0,#0x40]
lsr	r0,#24

return:
ldr	r3,=#0x809CF99
bx	r3
