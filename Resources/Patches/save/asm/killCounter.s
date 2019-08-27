.equ table, offset+4
.thumb
ldr	r0,[r1,#0x50]
mov	r7,#1
neg	r7,r7
cmp	r0,r7
beq	noIncrease
add	r0,#1
str	r0,[r1,#0x50]
noIncrease:
ldrb	r0,[r4]
push	{r0-r7}

ldrb	r0,[r5,#9]
cmp	r0,#0x4F
bhi	end
ldr	r1,table
ldrb	r1,[r1,r0]
cmp	r1,#0xFF
beq	end
ldr	r2,offset
ldrh	r0,[r2,r1]
ldr	r3,=#10000
cmp	r0,r3
beq	end
add	r0,#1
strh	r0,[r2,r1]

end:
pop	{r0-r7}
ldr	r7,=#0x804A5A5
bx	r7
.align
.ltorg
offset:
