.thumb
ldr	r3,getRNG
mov	lr,r3
.short	0xF800
mov	r1,#4
swi	#6
add	r1,#2
pop	{r0,r2,r3}
lsr	r0,r3,#8
push	{r3}
ldr	r3,=#0x805E950
mov	lr,r3
pop	{r3}
.short	0xF800
.align
.ltorg
getRNG:
