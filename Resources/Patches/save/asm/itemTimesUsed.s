.equ table, offset+4
.thumb
push	{r0-r7}
cmp	r4,#0x7F
bhi	end
ldr	r1,table
ldrb	r0,[r1,r4]
cmp	r0,#0xFF
beq	end
lsl	r0,#1
ldr	r1,offset
ldrh	r2,[r1,r0]
ldr	r3,=#10000
cmp	r2,r3
beq	end
add	r2,#1
strh	r2,[r1,r0]
end:
pop	{r0-r7}
ldrb	r0,[r5,#1]
strb	r0,[r1,#9]
strb	r4,[r1,#1]
strb	r6,[r1,#3]
pop	{r4-r6,pc}
.align
.ltorg
offset:
