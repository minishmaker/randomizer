.thumb
push	{lr}
mov	r3,r0
ldrb	r0,[r3,#9]
cmp	r0,#0x2E
bne	End
ldr	r0,newid
End:
lsl	r0,#3
push	{r3}
ldr	r3,=#0x806E698
mov	lr,r3
pop	{r3}
.short	0xF800

.align
.ltorg
newid:
