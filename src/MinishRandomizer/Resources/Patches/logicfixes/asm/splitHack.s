.thumb
push	{r0-r2}
ldr	r3,=#0x3000BF0
ldrb	r0,[r3,#4]
ldrb	r1,[r3,#5]
ldr	r3,poin
loop:
ldrb	r2,[r3]
cmp	r2,#0
beq	End
cmp	r2,r0
bne	Next
ldrb	r2,[r3,#1]
cmp	r2,#0xFF
beq	skipRoom
cmp	r2,r1
bne	Next
skipRoom:
ldrb	r2,[r3,#2]
cmp	r5,r2
blo	End
mov	r5,r2
b	End
Next:
add	r3,#3
b	loop

End:
pop	{r0-r2}
cmp	r4,r5
bcs	goto8073EE2

goto8073EDC:
ldr	r1,=#0x3004040
ldr	r3,=#0x8073EDD
bx	r3

goto8073EE2:
ldr	r3,=#0x8073EE3
bx	r3

.align
.ltorg
poin:
