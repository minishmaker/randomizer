.equ	return, newGameTable+4
.thumb
push	{r0-r7}

ldr	r4,newGameTable
bigloop:
ldr	r5,[r4]
cmp	r5,#0
beq	End
ldr	r6,[r4,#4]
ldr	r7,[r4,#8]
smallloop:
cmp	r7,#0
beq	nextbigloop
ldrb	r0,[r5]
strb	r0,[r6]
sub	r7,#1
add	r5,#1
add	r6,#1
b	smallloop
nextbigloop:
add	r4,#12
b	bigloop

End:
ldr	r3,return
mov	lr,r3
pop	{r0-r7}
.short	0xF800

.align
.ltorg
newGameTable:
@POIN newGameTable
@POIN return
